namespace YorkshireDigital.Web.Tests.Admin.ViewModels
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using NUnit.Framework;
    using YorkshireDigital.Data.Domain.Events;
    using YorkshireDigital.Data.Domain.Group;
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
                Price = 1.2m,
                Group = new Group
                {
                    Id = "existing-group",
                    Name = "Existing Group"
                },
                Talks = new List<EventTalk>
                {
                    new EventTalk { Id = 1, Link = "http://google.com", Speaker = "Bob", Title = "Super talk", Synopsis = "Super talk details"}
                }
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
            viewModel.GroupName.ShouldBeEquivalentTo("Existing Group");
            viewModel.GroupId.ShouldBeEquivalentTo("existing-group");
            viewModel.Talks.Count.ShouldBeEquivalentTo(1);
            viewModel.Talks[0].Id.ShouldBeEquivalentTo(1);
            viewModel.Talks[0].Link.ShouldBeEquivalentTo("http://google.com");
            viewModel.Talks[0].Speaker.ShouldBeEquivalentTo("Bob");
            viewModel.Talks[0].Title.ShouldBeEquivalentTo("Super talk");
            viewModel.Talks[0].Synopsis.ShouldBeEquivalentTo("Super talk details");
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
                Price = 1.2m,
                Talks = new List<AdminEventTalkViewModel>
                {
                    new AdminEventTalkViewModel { Id = 1, Link = "http://google.com", Speaker = "Bob", Title = "Super talk", Synopsis = "Super talk details"}
                }
            };

            // Act
            var domain = viewModel.ToDomain();

            // Assert
            domain.UniqueName.ShouldBeEquivalentTo("existing-event");
            domain.Title.ShouldBeEquivalentTo("Existing Event");
            domain.Synopsis.ShouldBeEquivalentTo("Existing event details");
            domain.Start.ShouldBeEquivalentTo(start.ToUniversalTime());
            domain.End.ShouldBeEquivalentTo(end.ToUniversalTime());
            domain.Location.ShouldBeEquivalentTo("Venue X");
            domain.Region.ShouldBeEquivalentTo("Leeds");
            domain.Price.ShouldBeEquivalentTo(1.2m);
            domain.Talks.Count.ShouldBeEquivalentTo(1);
            domain.Talks[0].Id.ShouldBeEquivalentTo(1);
            domain.Talks[0].Link.ShouldBeEquivalentTo("http://google.com");
            domain.Talks[0].Speaker.ShouldBeEquivalentTo("Bob");
            domain.Talks[0].Title.ShouldBeEquivalentTo("Super talk");
            domain.Talks[0].Synopsis.ShouldBeEquivalentTo("Super talk details");
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
                Price = 1.2m,
                Talks = new List<EventTalk>
                {
                    new EventTalk { Id = 1, Link = "http://google.com", Speaker = "Bob", Title = "Super talk", Synopsis = "Super talk"},
                    new EventTalk { Id = 2, Link = "http://yahoo.com", Speaker = "Greg", Title = "Greg talk", Synopsis = "Greg talk"}
                }
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
                Price = 2.2m,
                Talks = new List<AdminEventTalkViewModel>
                {
                    new AdminEventTalkViewModel { Id = 0, Link = "http://bing.com", Speaker = "Steve", Title = "New talk", Synopsis = "New talk details"},
                    new AdminEventTalkViewModel { Id = 2, Link = "http://yahoo.co.uk", Speaker = "Greg Smith", Title = "Greg Smith talk", Synopsis = "Greg Smith talk details"}
                }
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
            domain.Talks.Count.ShouldBeEquivalentTo(2);
            domain.Talks[0].Id.ShouldBeEquivalentTo(0);
            domain.Talks[0].Link.ShouldBeEquivalentTo("http://bing.com");
            domain.Talks[0].Speaker.ShouldBeEquivalentTo("Steve");
            domain.Talks[0].Title.ShouldBeEquivalentTo("New talk");
            domain.Talks[0].Synopsis.ShouldBeEquivalentTo("New talk details");
            domain.Talks[1].Id.ShouldBeEquivalentTo(2);
            domain.Talks[1].Link.ShouldBeEquivalentTo("http://yahoo.co.uk");
            domain.Talks[1].Speaker.ShouldBeEquivalentTo("Greg Smith");
            domain.Talks[1].Title.ShouldBeEquivalentTo("Greg Smith talk");
            domain.Talks[1].Synopsis.ShouldBeEquivalentTo("Greg Smith talk details");
        }
    }
}
