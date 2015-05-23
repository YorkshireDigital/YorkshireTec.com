namespace YorkshireDigital.Data.Domain.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using YorkshireDigital.Data.Domain.Account;
    using YorkshireDigital.Data.Domain.Group;
    using YorkshireDigital.Data.Domain.Shared;

    public class Event
    {
        public virtual string UniqueName { get; set; }
        public virtual Group Group { get; set; }
        public virtual string Title { get; set; }
        public virtual string Synopsis { get; set; }
        public virtual TextFormat SynopsisFormat { get; set; }
        public virtual DateTime Start { get; set; }
        public virtual DateTime End { get; set; }
        public virtual string Location { get; set; }
        public virtual string Region { get; set; }
        public virtual decimal Price { get; set; }
        public virtual byte[] Photo { get; set; }
        public virtual IList<Category> Categories { get; set; }
        public virtual IList<Interest> Interests { get; set; }
        public virtual IList<EventTalk> Talks { get; set; }
        public virtual DateTime LastEditedOn { get; set; }
        public virtual User LastEditedBy { get; set; }
        public virtual DateTime? DeletedOn { get; set; }
        public virtual User DeletedBy { get; set; }
        public virtual string MeetupId { get; set; }
        public virtual string EventSyncJobId { get; set; }

        public virtual bool IsDeleted { get { return DeletedOn.HasValue; } }

        // TODO: public virtual IList<User> Attendees { get; set; }

        public virtual void UpdateFromMeetup(MeetupApi.Models.Event meetupEvent)
        {
            Title = meetupEvent.Name;
            Synopsis = meetupEvent.Description;
            SynopsisFormat = TextFormat.Html;
            Start = meetupEvent.StartDate;
            End = meetupEvent.EndDate;
            
            if (meetupEvent.Venue == null) return;

            Location = meetupEvent.Venue.Address1;
            Region = meetupEvent.Venue.City;
        }

        public static Event FromMeetupGroup(MeetupApi.Models.Event upcomingEvent)
        {
            var newEvent = new Event
            {
                MeetupId = upcomingEvent.Id,
                DeletedBy = null,
                DeletedOn = null,
                End = upcomingEvent.Duration.HasValue
                    ? upcomingEvent.StartDate.AddMilliseconds(upcomingEvent.Duration.Value)
                    : upcomingEvent.StartDate,
                Start = upcomingEvent.StartDate
            };

            if (upcomingEvent.Group != null)
            {
                if (upcomingEvent.Group.Topics != null)
                {
                    newEvent.Interests = upcomingEvent.Group.Topics.Select(x => new Interest { Name = x.Name }).ToList();
                }
                if (upcomingEvent.Group.Category != null)
                {
                    newEvent.Categories = new List<Category> {new Category {Name = upcomingEvent.Group.Category.Name}};
                }
            }
            if (upcomingEvent.Venue != null)
            {
                newEvent.Location = upcomingEvent.Venue.Address1;
                newEvent.Region = upcomingEvent.Venue.City;
            }

            newEvent.LastEditedOn = DateTime.UtcNow;
            newEvent.Synopsis = upcomingEvent.Description;
            newEvent.SynopsisFormat = TextFormat.Html;
            newEvent.Title = upcomingEvent.Name;
            return newEvent;
        }
    }
}
