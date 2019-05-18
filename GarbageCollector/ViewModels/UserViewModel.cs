using System;

namespace GarbageCollector.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public string Login { get; set; }

        public TrashCanViewModel[] TrashCans { get; set; }

    }
}