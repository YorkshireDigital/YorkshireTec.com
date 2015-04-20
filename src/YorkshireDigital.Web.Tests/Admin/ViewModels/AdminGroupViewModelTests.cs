namespace YorkshireDigital.Web.Tests.Admin.ViewModels
{
    using FluentAssertions;
    using NUnit.Framework;
    using YorkshireDigital.Data.Domain.Organisations;
    using YorkshireDigital.Web.Admin.ViewModels;

    [TestFixture]
    public class AdminGroupViewModelTests
    {
        [Test]
        public void LoadFromDomain_MapsPropertiesCorrectly()
        {
            // Arrange
            var domain = new Group
            {
                Id = "existing-group",
                Name = "Existing Group",
                ShortName = "EXT",
                About = "This is an existing group",
                Colour = "#FF00FF",
                Headline = "Existing group for testing"
            };

            // Act
            var viewModel = AdminGroupViewModel.FromDomain(domain);

            // Assert
            viewModel.Id.ShouldBeEquivalentTo("existing-group");
            viewModel.Name.ShouldBeEquivalentTo("Existing Group");
            viewModel.ShortName.ShouldBeEquivalentTo("EXT");
            viewModel.About.ShouldBeEquivalentTo("This is an existing group");
            viewModel.Colour.ShouldBeEquivalentTo("#FF00FF");
            viewModel.Headline.ShouldBeEquivalentTo("Existing group for testing");
        }

        [Test]
        public void ToDomain_MapsPropertiesCorrectly()
        {
            // Arrange
            var viewModel = new AdminGroupViewModel
            {
                Id = "existing-group",
                Name = "Existing Group",
                ShortName = "EXT",
                About = "This is an existing group",
                Colour = "#FF00FF",
                Headline = "Existing group for testing"
            };

            // Act
            var domain = viewModel.ToDomain();

            // Assert
            domain.Id.ShouldBeEquivalentTo("existing-group");
            domain.Name.ShouldBeEquivalentTo("Existing Group");
            domain.ShortName.ShouldBeEquivalentTo("EXT");
            domain.About.ShouldBeEquivalentTo("This is an existing group");
            domain.Colour.ShouldBeEquivalentTo("#FF00FF");
            domain.Headline.ShouldBeEquivalentTo("Existing group for testing");
        }

        [Test]
        public void UpdateDomain_MapsPropertiesCorrectly()
        {
            // Arrange
            var domain = new Group
            {
                Id = "existing-group",
                Name = "Existing Group",
                ShortName = "EXT",
                About = "This is an existing group",
                Colour = "#FF00FF",
                Headline = "Existing group for testing"
            };

            var viewModel = new AdminGroupViewModel
            {
                Id = "updated-group",
                Name = "Updated Group",
                ShortName = "UPD",
                About = "This is an updated group",
                Colour = "#00FF00",
                Headline = "Updated group for testing"
            };

            // Act
            viewModel.UpdateDomain(domain);

            // Assert
            domain.Id.ShouldBeEquivalentTo("updated-group");
            domain.Name.ShouldBeEquivalentTo("Updated Group");
            domain.ShortName.ShouldBeEquivalentTo("UPD");
            domain.About.ShouldBeEquivalentTo("This is an updated group");
            domain.Colour.ShouldBeEquivalentTo("#00FF00");
            domain.Headline.ShouldBeEquivalentTo("Updated group for testing");
        }
    }
}
