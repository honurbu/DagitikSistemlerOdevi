using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TraversalApp.Repository.Context;

namespace TraversalApp.MVC.ViewComponents.AdminDashboard
{
    public class _AdminGuideList : ViewComponent
    {
        private readonly AppDbContext _context;

        public _AdminGuideList(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _context.Guides.ToListAsync();
            return View(values);
        }
    }
}
