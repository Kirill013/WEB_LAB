using System;
using System.Collections.Generic;
using BookStore.Models;
using BookStore.Models.Repository;
using System.Linq;
using BookStore.Pages.Helpers;
using System.Web.Routing;

namespace BookStore.Pages
{
    public partial class Listing : System.Web.UI.Page
    {
        private Repository repository = new Repository();
        private int pageSize = 6;

        protected int CurrentPage
        {
            get
            {
                int page;
                page = GetPageFromRequest();
                return page > MaxPage ? MaxPage : page;
            }
        }

        protected int MaxPage
        {
            get 
            {
                int prodCount = FilterBooks().Count();
                return (int)Math.Ceiling((decimal)prodCount / pageSize);
            }
        }

        private int GetPageFromRequest()
        {
            int page;
            string reqValue = (string)RouteData.Values["page"] ??
                Request.QueryString["page"];
            return reqValue != null && int.TryParse(reqValue, out page) ? page : 1;
        }

        public IEnumerable<Book> GetBooks()
        {
            return FilterBooks()
                .OrderBy(g => g.BookId)
                .Skip((CurrentPage - 1) * pageSize)
                .Take(pageSize);
        }

        // Новый вспомогательный метод для фильтрации игр по категориям
        private IEnumerable<Book> FilterBooks()
        {
            IEnumerable<Book> books = repository.Books;
            string currentCategory = (string)RouteData.Values["category"] ??
                Request.QueryString["category"]; 
            return currentCategory == null ? books :
                books.Where(p => p.Category == currentCategory);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                int selectedGameId;
                if (int.TryParse(Request.Form["add"], out selectedGameId))
                {
                    Book selectedGame = repository.Books
                        .Where(g => g.BookId == selectedGameId).FirstOrDefault();

                    if (selectedGame != null)
                    {
                        SessionHelper.GetCart(Session).AddItem(selectedGame, 1);
                        SessionHelper.Set(Session, SessionKey.RETURN_URL,
                            Request.RawUrl);

                        Response.Redirect(RouteTable.Routes
                            .GetVirtualPath(null, "cart", null).VirtualPath);
                    }
                }
            }
        }
    }
}