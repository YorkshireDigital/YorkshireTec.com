namespace YorkshireDigital.Web.Admin.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using YorkshireDigital.Data.Domain.Account;
    using YorkshireDigital.Data.Domain.Events;
    using YorkshireDigital.Data.Domain.Group;

    public class AdminIndexViewModel
    {
        public IList<AdminUserListViewModel> Users { get; set; }
        public IList<AdminGroupListViewModel> Groups { get; set; }
        public IList<AdminEventListViewModel> FutureEvents { get; set; }
        public IList<AdminEventListViewModel> PastEvents { get; set; }

        public AdminIndexViewModel(IEnumerable<User> users, IEnumerable<Event> events, IEnumerable<Group> groups)
        {
            Users = new List<AdminUserListViewModel>();
            Groups = new List<AdminGroupListViewModel>();
            FutureEvents = new List<AdminEventListViewModel>();
            PastEvents = new List<AdminEventListViewModel>();

            foreach (var user in users)
            {
                Users.Add(Mapper.DynamicMap<AdminUserListViewModel>(user));
            }

            foreach (var @group in groups)
            {
                Groups.Add(Mapper.DynamicMap<AdminGroupListViewModel>(@group));
            }

            foreach (var @event in events.Where(x => x.Start < DateTime.UtcNow))
            {
                PastEvents.Add(Mapper.DynamicMap<AdminEventListViewModel>(@event));
            }

            foreach (var @event in events.Where(x => x.Start >= DateTime.UtcNow))
            {
                FutureEvents.Add(Mapper.DynamicMap<AdminEventListViewModel>(@event));
            }
        }
    }
}