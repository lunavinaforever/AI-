using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace BilibiliVideoSearch
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchText = searchTextBox.Text;
            await SearchBilibiliVideosAsync(searchText);
        }

        private async Task SearchBilibiliVideosAsync(string searchText)
        {
            var httpClient = new HttpClient();
            try
            {
                // 替换以下 URL 为实际的 Bilibili API URL
                string requestUri = $"https://api.bilibili.com/x/web-interface/search/type?search_type=video&keyword={Uri.EscapeDataString(searchText)}";
                
                var response = await httpClient.GetStringAsync(requestUri);
                var videos = JsonConvert.DeserializeObject<BilibiliApiResponse>(response);

                // 更新 UI，显示搜索结果
                Dispatcher.Invoke(() =>
                {
                    resultsListView.Items.Clear();
                    foreach (var video in videos.data.result)
                    {
                        resultsListView.Items.Add(new { Title = video.title, Description = video.description });
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            finally
            {
                httpClient.Dispose();
            }
        }

        // 定义根据 Bilibili API 响应格式定义的数据模型
        public class BilibiliApiResponse
        {
            public Data data { get; set; }
        }

        public class Data
        {
            public List<VideoResult> result { get; set; }
        }

        public class VideoResult
        {
            public string title { get; set; }
            public string description { get; set; }
            // 根据 API 响应添加更多属性
        }
    }
}
