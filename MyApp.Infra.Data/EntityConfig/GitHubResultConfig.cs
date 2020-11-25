using MyApp.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace MyApp.Infra.Data.EntityConfig
{
    class GitHubResultConfig : EntityTypeConfiguration<GitHubResult>
    {
        //Configure GitHubResultConfig
        public GitHubResultConfig()
        {
            //Set the primary key
            HasKey(c => c.GitHubResultId);

            //Informs that UserGitHub is required and must have a maximum of 255 characters
            Property(c => c.UserGitHub)
                .IsRequired()
                .HasMaxLength(255);

            //Informs that RepositoryGitHub is required and must have a maximum of 255 characters
            Property(c => c.RepositoryGitHub)
                .IsRequired()
                .HasMaxLength(255);

            //Informs that CommitToken is required and must have a maximum of 255 characters
            Property(c => c.CommitToken)
                .IsRequired()
                .HasMaxLength(255);

            //Informs that LastCommit is required
            Property(c => c.LastCommit)
                .IsRequired();

            //Informs that the ResultGitHub is required and is of the long text type
            Property(c => c.ResultGitHub)
                .IsRequired()
                .HasColumnType("longtext");

        }
    }
}
