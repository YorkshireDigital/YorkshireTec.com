namespace YorkshireDigital.Web.Tests.Admin.ViewModels
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using YorkshireDigital.Data.Domain.Events;
    using YorkshireDigital.Web.Admin.ViewModels;

    public class AdminEventViewModelTests
    {
        [Test]
        public void LoadFromDomain_MapsPropertiesCorrectly()
        {
            // Arrange
            var start = DateTime.UtcNow.AddHours(-1);
            var end = DateTime.UtcNow.AddHours(1);
            var domain = new Event
            {
                UniqueName = "existing-event",
                Title = "Existing Event",
                Synopsis = "Existing event details",
                Start = start,
                End = end,
                Location = "Venue X",
                Region = "Leeds",
                Price = 1.2m
            };

            // Act
            var viewModel = AdminEventViewModel.FromDomain(domain);

            // Assert
            viewModel.UniqueName.ShouldBeEquivalentTo("existing-event");
            viewModel.Title.ShouldBeEquivalentTo("Existing Event");
            viewModel.Synopsis.ShouldBeEquivalentTo("Existing event details");
            viewModel.Start.ShouldBeEquivalentTo(start);
            viewModel.End.ShouldBeEquivalentTo(end);
            viewModel.Location.ShouldBeEquivalentTo("Venue X");
            viewModel.Region.ShouldBeEquivalentTo("Leeds");
            viewModel.Price.ShouldBeEquivalentTo(1.2m);
        }

        [Test]
        public void ToDomain_MapsPropertiesCorrectly()
        {
            // Arrange
            var start = DateTime.UtcNow.AddHours(-1);
            var end = DateTime.UtcNow.AddHours(1);
            var viewModel = new AdminEventViewModel
            {
                UniqueName = "existing-event",
                Title = "Existing Event",
                Synopsis = "Existing event details",
                Start = start,
                End = end,
                Location = "Venue X",
                Region = "Leeds",
                Price = 1.2m
            };

            // Act
            var domain = viewModel.ToDomain();

            // Assert
            domain.UniqueName.ShouldBeEquivalentTo("existing-event");
            domain.Title.ShouldBeEquivalentTo("Existing Event");
            domain.Synopsis.ShouldBeEquivalentTo("Existing event details");
            domain.Start.ShouldBeEquivalentTo(start);
            domain.End.ShouldBeEquivalentTo(end);
            domain.Location.ShouldBeEquivalentTo("Venue X");
            domain.Region.ShouldBeEquivalentTo("Leeds");
            domain.Price.ShouldBeEquivalentTo(1.2m);
        }

        [Test]
        public void UpdateDomain_MapsPropertiesCorrectly()
        {
            // Arrange
            var start = DateTime.UtcNow.AddHours(-1);
            var end = DateTime.UtcNow.AddHours(1);
            var domain = new Event
            {
                UniqueName = "existing-event",
                Title = "Existing Event",
                Synopsis = "Existing event details",
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow,
                Location = "Venue X",
                Region = "Leeds",
                Price = 1.2m
            };

            var viewModel = new AdminEventViewModel
            {
                UniqueName = "existing-event",
                Title = "Updated Event",
                Synopsis = "Updated event details",
                Start = start,
                End = end,
                Location = "Venue Y",
                Region = "York",
                Price = 2.2m
            };

            // Act
            viewModel.UpdateDomain(domain);

            // Assert
            domain.UniqueName.ShouldBeEquivalentTo("existing-event");
            domain.Title.ShouldBeEquivalentTo("Updated Event");
            domain.Synopsis.ShouldBeEquivalentTo("Updated event details");
            domain.Start.ShouldBeEquivalentTo(start);
            domain.End.ShouldBeEquivalentTo(end);
            domain.Location.ShouldBeEquivalentTo("Venue Y");
            domain.Region.ShouldBeEquivalentTo("York");
            domain.Price.ShouldBeEquivalentTo(2.2m);
        }
    }
}
