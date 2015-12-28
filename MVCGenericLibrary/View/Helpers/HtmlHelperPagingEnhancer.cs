namespace MVCGenericLibrary.View.Helpers
{
    using System;
    using System.Text;
    using System.Web.Mvc;

    public static class HtmlHelperPagingEnhancer
    {


        public static string PageLinksNumbered(this HtmlHelper html, int currentPage, int totalPages, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            TagBuilder tag = new TagBuilder("a"); // Construct an <a> tag
            for (int i = 1; i <= totalPages; i++)
            {
                tag.MergeAttribute("href", pageUrl(i), true);
                tag.InnerHtml = i.ToString();
                if (i == currentPage)
                    tag.AddCssClass("selected");
                result.AppendLine(tag.ToString());
            }
            return result.ToString();
        }

        public static string PageLinksPrevNext(this HtmlHelper html, int currentPage, int totalPages, Func<int, string> pageUrl)
        {
            return PageLinksPrevNext(html, currentPage, totalPages, pageUrl, "<<<", ">>>");
        }

        public static string PageLinksPrevNext(this HtmlHelper html, int currentPage, int totalPages, Func<int, string> pageUrl, string prevLinkContent, string nextLinkContent)
        {
            StringBuilder result = new StringBuilder();
            TagBuilder tag = new TagBuilder("a"); // Construct an <a> tag
            
            if (currentPage > 1)
            {
                tag.MergeAttribute("href", pageUrl(currentPage - 1));
                tag.InnerHtml = prevLinkContent;
                result.AppendLine(tag.ToString());
                //result.Append(html.RouteLink("<<<", "PagedIndexData", new { page = currentPage - 1 }));
            }
            result.Append(" ");

            if (currentPage < totalPages)
            {
                tag.MergeAttribute("href", pageUrl(currentPage + 1), true);
                tag.InnerHtml = nextLinkContent;
                result.AppendLine(tag.ToString());
                //result.Append(html.RouteLink(">>>", "PagedIndexData", new { page = currentPage + 1 }));
            }

            return result.ToString();
        }
    }
}