﻿using BulbaCourse.Video.Data.DatabaseContex;
using BulbaCourse.Video.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Video.Data.Models;

namespace BulbaCourse.Video.Data.Repositories
{
    public class TegRepository : ITegRepository
    {
        private readonly VideoDbContext videoDbContext;

        public TegRepository(VideoDbContext videoDbContext)
        {
            this.videoDbContext = videoDbContext;
        }
        public void Add(TagDb tag)
        {
            videoDbContext.Tags.Add(tag);
            videoDbContext.SaveChanges();
        }

        public IEnumerable<TagDb> GetAll()
        {
            var tagList = videoDbContext.Tags.ToList().AsReadOnly();
            return tagList;
        }

        public TagDb GetById(string tagId)
        {
            var tag = videoDbContext.Tags.FirstOrDefault(b => b.TagId.Equals(tagId));
            return tag;
        }

        public void Remove(TagDb tag)
        {
            videoDbContext.Tags.Remove(tag);
            videoDbContext.SaveChanges();
        }

        public void RemoveById(string tagId)
        {
            var deletedTag = videoDbContext.Tags.FirstOrDefault(b => b.TagId.Equals(tagId));
            Remove(deletedTag);
        }

        public void Update(TagDb tag)
        {
            var oldTag = videoDbContext.Tags.FirstOrDefault(b => b.TagId.Equals(tag.TagId));
            oldTag = tag;
            videoDbContext.SaveChanges();
        }
    }
}
