namespace YorkshireDigital.Data.Tests.Domain.Events
{
    using System;
    using FluentAssertions;
    using NUnit.Framework;
    using YorkshireDigital.MeetupApi.Helpers;
    using YorkshireDigital.MeetupApi.Models;
    using Event = YorkshireDigital.Data.Domain.Events.Event;

    [TestFixture]
    public class EventTests
    {
        [Test]
        public void UpdateFromMeetup_UpdatesDomainModel()
        {
            // Arrange
            var domain = new Event
            {
                Title = "Old Title",
                Synopsis = "Old Synopsis",
                Start = DateTime.UtcNow.AddDays(1),
                End = DateTime.UtcNow.AddDays(1),
                Location = "Old Location",
                Region = "Old Region"
            };
            var start = DateTime.UtcNow.AddDays(1).AddHours(2);
            var meetup = new MeetupApi.Models.Event
                        {
                            Name = "New name",
                            Id = "12345",
                            Description = "New description",
                            Duration = 3600000,
                            Time = DateHelpers.DateTimeToMeetupTimeStamp(start),
                            Venue = new Venue
                            {
                                Address1 = "New Address",
                                City = "New City"
                            }
                        };

            // Act
            domain.UpdateFromMeetup(meetup);

            // Assert
            domain.Title.ShouldBeEquivalentTo("New name");
            domain.Synopsis.ShouldBeEquivalentTo("New description");
            domain.Start.Should().BeCloseTo(start);
            domain.End.Should().BeCloseTo(start.AddHours(1));
            domain.Location.ShouldBeEquivalentTo("New Address");
            domain.Region.ShouldBeEquivalentTo("New City");
        }
    }
}
