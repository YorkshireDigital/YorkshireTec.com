namespace YorkshireDigital.Web.Tests.Admin.ViewModels
{
    using FluentAssertions;
    using NUnit.Framework;
    using YorkshireDigital.Data.Domain.Events;
    using YorkshireDigital.Web.Admin.ViewModels;

    [TestFixture]
    public class AdminInterestViewModelTests
    {
        [Test]
        public void LoadFromDomain_MapsPropertiesCorrectly()
        {
            // Arrange
            var domain = new Interest
            {
                Id = 1,
                Name = "Interest",
            };

            // Act
            var viewModel = AdminInterestViewModel.FromDomain(domain, new Interest[0]);

            // Assert
            viewModel.Id.ShouldBeEquivalentTo(1);
            viewModel.Name.ShouldBeEquivalentTo("Interest");
            viewModel.Selected.Should().BeFalse();
        }

        [Test]
        public void LoadFromDomain_WhenInterestIsInList_SelectedIsTrue()
        {
            // Arrange
            var domain = new Interest
            {
                Id = 1,
                Name = "Interest",
            };

            // Act
            var viewModel = AdminInterestViewModel.FromDomain(domain, new[]{ new Interest { Id = 1, Name = "Interest"} });

            // Assert
            viewModel.Id.ShouldBeEquivalentTo(1);
            viewModel.Name.ShouldBeEquivalentTo("Interest");
            viewModel.Selected.Should().BeTrue();
        }

        [Test]
        public void ToDomain_MapsPropertiesCorrectly()
        {
            // Arrange
            var viewModel = new AdminInterestViewModel
            {
                Id = 1,
                Name = "Interest",
            };

            // Act
            var domain = viewModel.ToDomain();

            // Assert
            domain.Id.ShouldBeEquivalentTo(1);
            domain.Name.ShouldBeEquivalentTo("Interest");
        }
    }
}
