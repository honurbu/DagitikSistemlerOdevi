﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraversalApp.Core.Entites;

namespace TraversalApp.Core.Repositories
{
    public interface IContactUsRepository : IGenericRepository<ContactUs>
    {
        List<ContactUs> GetListContactUsByTrue();
        List<ContactUs> GetListContactUsByFalse();
        void ContactUsStatusChangeToTrue(int id);
        void ContactUsStatusChangeToFalse(int id);
    }
}
