using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using VKApiSchemaParser.Models.Schemas;

namespace VKApiSchemaParser.Tests
{
    class Program
    {
        public static void Main(string[] args)
        {
            CheckObjects();
            CheckResonses();
            CheckMethods();
        }

        public static void CheckObjects()
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
            a.Objects = a.Objects.Where(o => testSet.Contains(o.Key)).ToDictionary(o => o.Key, o => o.Value);

            var serializedSchema = SerializeObject(a);
            WriteToFile(serializedSchema, "objects");
        }

        public static void CheckResonses()
        {
            var testSet = new string[]
                {
                "ok_response",
                "account_changePassword_response",
                "account_getActiveOffers_response",
                "account_getAppPermissions_response",
                "account_getBanned_response",
                "account_saveProfileInfo_response",
                "ads_createCampaigns_response",
                "auth_confirm_response",
                "board_getTopics_response",
                "board_getTopics_extended_response",
                "database_getRegions_response",
                "friends_delete_response",
                "friends_addList_response",
                "friends_delete_response",
                "newsfeed_getSuggestedSources_response",
                "messages_delete_response",
                "users_getSubscriptions_extended_response"
                };

            var vkapi = new VKApiSchema();
            var a = vkapi.GetResponsesAsync().Result;
            a.Responses = a.Responses.Where(o => testSet.Contains(o.Key)).ToDictionary(o => o.Key, o => o.Value);

            var serializedSchema = SerializeObject(a);
            WriteToFile(serializedSchema, "responses");
        }

        public static void CheckMethods()
        {
            var testSet = new string[]
                {
                "users.get",
                "users.getSubscriptions",
                "users.getFollowers",
                "auth.checkPhone",
                "users.search",
                "friends.getByPhones",
                "friends.areFriends",
                "friends.getAvailableForCall"
                };

            var vkapi = new VKApiSchema();
            var a = vkapi.GetMethodsAsync().Result;
            a.Methods = a.Methods.Where(o => testSet.Contains(o.OriginalName)).ToList();

            var serializedSchema = SerializeObject(a);
            WriteToFile(serializedSchema, "methods");
        }

        private static string SerializeObject(IApiSchema schema)
        {
            return JsonConvert.SerializeObject(schema, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            });
        }

        private static void WriteToFile(string data, string prefix)
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var filePath = Path.Combine(desktopPath, $"{prefix}-{DateTime.Now.ToString("HHmmss")}.json");
            File.WriteAllText(filePath, data);
        }
    }
}
