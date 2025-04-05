using System.Collections.Generic;

namespace api.Dtos.News
{
    public class NewsApiResponseDto
    {
        public List<NewsArticleDto> Articles { get; set; }
    }
}