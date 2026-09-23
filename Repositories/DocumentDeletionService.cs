using cv_api.Models;

namespace cv_api.Repositories;

public sealed class DocumentDeletionService(
    IRepository<Profile> profiles,
    IRepository<Resume> resumes,
    IRepository<Skill> skills,
    IRepository<Experience> experiences,
    IRepository<Education> education,
    IRepository<Project> projects)
{
    public async Task<DeleteConflict?> GetConflictAsync<T>(
        string id,
        CancellationToken cancellationToken)
        where T : class
    {
        if (typeof(T) == typeof(PersonalInfo))
        {
            var profile = await profiles.FirstOrDefaultAsync(
                item => item.PersonalInfoId == id,
                cancellationToken);

            if (profile is not null)
            {
                return new DeleteConflict(
                    "Personal info is used by a profile. Delete the profile first.");
            }
        }

        if (typeof(T) == typeof(Profile))
        {
            var resume = await resumes.FirstOrDefaultAsync(
                item => item.ProfileId == id,
                cancellationToken);

            if (resume is not null)
            {
                return new DeleteConflict(
                    "Profile is used by a resume. Delete the resume first.");
            }
        }

        if (typeof(T) == typeof(SkillCategory))
        {
            var skill = await skills.FirstOrDefaultAsync(
                item => item.CategoryId == id,
                cancellationToken);

            if (skill is not null)
            {
                return new DeleteConflict(
                    "Skill category is used by a skill. Delete the skill first.");
            }
        }

        return null;
    }

    public async Task CleanupReferencesAsync<T>(
        string id,
        CancellationToken cancellationToken)
        where T : class
    {
        if (typeof(T) == typeof(Experience))
        {
            await UpdateAsync(
                profiles,
                profile => profile.Experiences?.RemoveAll(item => item.ExperienceId == id) > 0,
                cancellationToken);
            return;
        }

        if (typeof(T) == typeof(Education))
        {
            await UpdateAsync(
                profiles,
                profile => profile.EducationIds?.RemoveAll(item => item == id) > 0,
                cancellationToken);
            return;
        }

        if (typeof(T) == typeof(Project))
        {
            await UpdateAsync(
                profiles,
                profile => profile.ProjectIds?.RemoveAll(item => item == id) > 0,
                cancellationToken);
            return;
        }

        if (typeof(T) == typeof(SpokenLanguage))
        {
            await UpdateAsync(
                profiles,
                profile => profile.SpokenLanguageIds?.RemoveAll(item => item == id) > 0,
                cancellationToken);
            return;
        }

        if (typeof(T) != typeof(Skill))
        {
            return;
        }

        await UpdateAsync(
            profiles,
            profile => profile.SkillIds?.RemoveAll(item => item == id) > 0,
            cancellationToken);
        await UpdateAsync(
            experiences,
            experience => experience.SkillIds?.RemoveAll(item => item == id) > 0,
            cancellationToken);
        await UpdateAsync(
            education,
            item => item.SkillIds?.RemoveAll(skillId => skillId == id) > 0,
            cancellationToken);
        await UpdateAsync(
            projects,
            project => project.SkillIds?.RemoveAll(item => item == id) > 0,
            cancellationToken);
    }

    private static async Task UpdateAsync<T>(
        IRepository<T> repository,
        Func<T, bool> removeReference,
        CancellationToken cancellationToken)
        where T : class
    {
        var documents = await repository.GetAllAsync(cancellationToken);

        foreach (var document in documents)
        {
            if (!removeReference(document))
            {
                continue;
            }

            var documentId = DocumentId.Get(document);
            DocumentTimestamps.SetUpdated(document, document, DateTime.UtcNow);
            await repository.UpdateAsync(documentId, document, cancellationToken);
        }
    }
}

public sealed record DeleteConflict(string Message);
