using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using VKApiSchemaParser;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            var testSet = new string[]
            {
                "account_account_counters",
                "account_lookup_result",
                "account_name_request_status",
                "account_name_request",
                "account_onoff_options",
                "account_push_conversations",
                "account_push_params",
                "account_push_settings",
                "account_user_xtr_contact",
                "ads_account",
                "ads_ad_cost_type",
                "ads_ad_approved",
                "ads_ad",
                "ads_ad_layout",
                "ads_campaign_type",
                "ads_criteria",
                "ads_stats_sex_value",
                "ads_targ_settings",
                "audio_audio_full",
                "base_bool_int",
                "base_link_button_action_type",
                "base_ok_response",
                "friends_requests_mutual",
                "friends_friend_status",
                "groups_cover",
                "groups_group_full",
                "groups_user_xtr_role",
                "newsfeed_newsfeed_item",
                "notifications_notification_parent",
                "video_video_files"
            };

            var vkapi = new VKApiSchema();
            var a = vkapi.GetObjectsAsync().Result;
            a.Objects = a.Objects.Where(o => testSet.Contains(o.OriginalName));
            var j = JsonConvert.SerializeObject(a, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            });

            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var filePath = Path.Combine(desktopPath, $"json-{DateTime.Now.ToString("ddMMyyHHmmss")}.json");
            File.WriteAllText(filePath, j);
            Console.WriteLine();
        }
    }
}
