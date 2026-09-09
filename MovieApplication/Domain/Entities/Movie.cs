using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using CorePersistence.Repositories;
using CoreSecurity.Entities;

namespace Domain.Entities
{
    public class Movie : Entity<Guid>
    {

        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string VideoURL { get; set; }
        public string Description { get; set; }
        public string ProducerName { get; set; }
        public int ReleaseDate { get; set; }

        public int PublisherId { get; set; }
        public User Publisher { get; set; }

        public ICollection<MoviesCategory> MoviesCategories { get; set; }
        public ICollection<Comments> Comments { get; set; }
        public ICollection<SavedMovie> SavedMovies { get; set; }
        public ICollection<LikedMovie> LikedMovies { get; set; }
        public ICollection<Activities> Activities { get; set; }


        public Movie() { }

        public Movie(Guid Id, string name, string ımageURL, string videoURL, string description, string producerName, int publisherId) : this()
        {
            EntityID = Id;
            Name = name;
            ImageURL = ımageURL;
            VideoURL = videoURL;
            Description = description;
            ProducerName = producerName;
            PublisherId = publisherId;
        }



    }
}
