using cv_api.Dtos;
using cv_api.Models;
using cv_api.Repositories;

namespace cv_api.Queries;

public class QueryService(
    IRepository<Resume> resumes,
    IRepository<Profile> profiles,
    IRepository<PersonalInfo> personalInfo,
    IRepository<Experience> experiences,
    IRepository<Education> education,
    IRepository<Project> projects,
    IRepository<Skill> skills,
    IRepository<SkillCategory> skillCategories,
    IRepository<SpokenLanguage> spokenLanguages
)
{
    public async Task<ResumeByLinkResponse?> GetResumeDetailsAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        var resume = await resumes.GetByIdAsync(id, cancellationToken);

        return resume is null
            ? null
            : await BuildResumeResponseAsync(resume, cancellationToken);
    }

    public async Task<ResumeByLinkResponse?> GetResumeByLinkAsync(
        string linkId,
        CancellationToken cancellationToken = default
    )
    {
        var resume = await resumes.FirstOrDefaultAsync(
            resume => resume.LinkId == linkId,
            cancellationToken
        );

        if (resume is null)
        {
            return null;
        }

        return await BuildResumeResponseAsync(resume, cancellationToken);
    }

    public async Task<ProfileDetailsResponse?> GetProfileDetailsAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        var profile = await profiles.GetByIdAsync(id, cancellationToken);

        if (profile is null)
        {
            return null;
        }

        var profilePersonalInfo = await personalInfo.GetByIdAsync(
            profile.PersonalInfoId,
            cancellationToken
        );
        var profileExperiences = await GetExperienceDetailsAsync(
            profile.Experiences.Select(experience => experience.ExperienceId),
            cancellationToken
        );
        var profileEducation = await GetEducationDetailsAsync(profile.EducationIds, cancellationToken);
        var profileProjects = await GetProjectDetailsAsync(profile.ProjectIds, cancellationToken);
        var profileSkills = await GetSkillDetailsAsync(profile.SkillIds, cancellationToken);
        var profileSpokenLanguages = await GetByIdsAsync(
            spokenLanguages,
            profile.SpokenLanguageIds,
            cancellationToken
        );

        return new ProfileDetailsResponse(
            profile,
            profilePersonalInfo,
            profileExperiences,
            profileEducation,
            profileProjects,
            profileSkills,
            profileSpokenLanguages
        );
    }

    public async Task<ExperienceDetailsResponse?> GetExperienceDetailsAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        var experience = await experiences.GetByIdAsync(id, cancellationToken);

        return experience is null
            ? null
            : await BuildExperienceDetailsAsync(experience, cancellationToken);
    }

    public async Task<EducationDetailsResponse?> GetEducationDetailsAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        var educationDocument = await education.GetByIdAsync(id, cancellationToken);

        return educationDocument is null
            ? null
            : await BuildEducationDetailsAsync(educationDocument, cancellationToken);
    }

    public async Task<ProjectDetailsResponse?> GetProjectDetailsAsync(
        string id,
        CancellationToken cancellationToken = default
    )
    {
        var project = await projects.GetByIdAsync(id, cancellationToken);

        return project is null
            ? null
            : await BuildProjectDetailsAsync(project, cancellationToken);
    }

    private async Task<IReadOnlyCollection<ExperienceDetailsResponse>> GetExperienceDetailsAsync(
        IEnumerable<string> ids,
        CancellationToken cancellationToken
    )
    {
        var documents = await GetByIdsAsync(experiences, ids, cancellationToken);
        var result = new List<ExperienceDetailsResponse>();

        foreach (var document in documents)
        {
            result.Add(await BuildExperienceDetailsAsync(document, cancellationToken));
        }

        return result;
    }

    private async Task<ResumeByLinkResponse> BuildResumeResponseAsync(
        Resume resume,
        CancellationToken cancellationToken
    )
    {
        var profile = await GetProfileDetailsAsync(resume.ProfileId, cancellationToken);

        return new ResumeByLinkResponse(resume, profile);
    }

    private async Task<IReadOnlyCollection<EducationDetailsResponse>> GetEducationDetailsAsync(
        IEnumerable<string> ids,
        CancellationToken cancellationToken
    )
    {
        var documents = await GetByIdsAsync(education, ids, cancellationToken);
        var result = new List<EducationDetailsResponse>();

        foreach (var document in documents)
        {
            result.Add(await BuildEducationDetailsAsync(document, cancellationToken));
        }

        return result;
    }

    private async Task<IReadOnlyCollection<ProjectDetailsResponse>> GetProjectDetailsAsync(
        IEnumerable<string> ids,
        CancellationToken cancellationToken
    )
    {
        var documents = await GetByIdsAsync(projects, ids, cancellationToken);
        var result = new List<ProjectDetailsResponse>();

        foreach (var document in documents)
        {
            result.Add(await BuildProjectDetailsAsync(document, cancellationToken));
        }

        return result;
    }

    private async Task<ExperienceDetailsResponse> BuildExperienceDetailsAsync(
        Experience experience,
        CancellationToken cancellationToken
    )
    {
        var experienceSkills = await GetSkillDetailsAsync(experience.SkillIds, cancellationToken);

        return new ExperienceDetailsResponse(experience, experienceSkills);
    }

    private async Task<EducationDetailsResponse> BuildEducationDetailsAsync(
        Education education,
        CancellationToken cancellationToken
    )
    {
        var educationSkills = await GetSkillDetailsAsync(education.SkillIds, cancellationToken);

        return new EducationDetailsResponse(education, educationSkills);
    }

    private async Task<ProjectDetailsResponse> BuildProjectDetailsAsync(
        Project project,
        CancellationToken cancellationToken
    )
    {
        var projectSkills = await GetSkillDetailsAsync(project.SkillIds, cancellationToken);

        return new ProjectDetailsResponse(project, projectSkills);
    }

    private async Task<IReadOnlyCollection<SkillDetailsResponse>> GetSkillDetailsAsync(
        IEnumerable<string> ids,
        CancellationToken cancellationToken
    )
    {
        var orderedIds = Distinct(ids);

        if (orderedIds.Count == 0)
        {
            return [];
        }

        var skillDocuments = await skills.FindAsync(
            skill => skill.Id != null && orderedIds.Contains(skill.Id),
            cancellationToken
        );
        var orderedSkills = OrderByIds(skillDocuments, orderedIds);
        var categoryIds = Distinct(orderedSkills.Select(skill => skill.CategoryId));
        var categoryDocuments = await skillCategories.FindAsync(
            category => category.Id != null && categoryIds.Contains(category.Id),
            cancellationToken
        );
        var categoriesById = categoryDocuments
            .Where(category => category.Id is not null)
            .ToDictionary(category => category.Id!, StringComparer.Ordinal);

        return orderedSkills
            .Select(skill => new SkillDetailsResponse(
                skill,
                categoriesById.GetValueOrDefault(skill.CategoryId)
            ))
            .ToList();
    }

    private static async Task<IReadOnlyCollection<T>> GetByIdsAsync<T>(
        IRepository<T> repository,
        IEnumerable<string> ids,
        CancellationToken cancellationToken
    )
        where T : class
    {
        var orderedIds = Distinct(ids);

        if (orderedIds.Count == 0)
        {
            return [];
        }

        var documents = await repository.GetByIdsAsync(orderedIds, cancellationToken);

        return OrderByIds(documents, orderedIds);
    }

    private static List<T> OrderByIds<T>(IEnumerable<T> documents, IReadOnlyCollection<string> ids)
    {
        var documentsById = documents
            .Select(document => new
            {
                Id = GetDocumentId(document),
                Document = document
            })
            .Where(item => item.Id is not null)
            .ToDictionary(item => item.Id!, item => item.Document, StringComparer.Ordinal);

        return ids
            .Where(documentsById.ContainsKey)
            .Select(id => documentsById[id])
            .ToList();
    }

    private static List<string> Distinct(IEnumerable<string> ids)
    {
        return ids
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Distinct(StringComparer.Ordinal)
            .ToList();
    }

    private static string? GetDocumentId<T>(T document)
    {
        return typeof(T).GetProperty("Id")?.GetValue(document) as string;
    }
}
