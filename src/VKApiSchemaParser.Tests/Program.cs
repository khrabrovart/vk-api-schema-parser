using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VKApiSchemaParser.Tests
{
    public class Program
    {
        private static readonly string[] _objectsTestSet =
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
            "groups_address",
            "groups_cover",
            "groups_group_full",
            "groups_user_xtr_role",
            "newsfeed_newsfeed_item",
            "notifications_notification_parent",
            "video_video_files",
            "users_fields"
        };

        private static readonly string[] _responsesTestSet =
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

        private static readonly string[] _methodsTestSet =
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

        public static async Task Main(string[] args)
        {
            await CheckObjectsAsync();
            await CheckResonsesAsync();
            await CheckMethodsAsync();
        }

        public static async Task CheckObjectsAsync()
        {
            var objects = (await VKApiSchema.ParseAsync()).Objects;
            //objects = objects.Where(o => _objectsTestSet.Contains(o.Key)).ToDictionary(o => o.Key, o => o.Value);

            var serializedSchema = SerializeObject(objects);
            //await SaveToFileAsync(serializedSchema, "objects");
        }

        public static async Task CheckResonsesAsync()
        {
            var responses = (await VKApiSchema.ParseAsync()).Responses;
            //responses = responses.Where(o => _responsesTestSet.Contains(o.Key)).ToDictionary(r => r.Key, r => r.Value);

            var serializedSchema = SerializeObject(responses);
            //await SaveToFileAsync(serializedSchema, "responses");
        }

        public static async Task CheckMethodsAsync()
        {
            var methods = (await VKApiSchema.ParseAsync()).Methods;
            //methods = methods.Where(o => _methodsTestSet.Contains(o.Key)).ToDictionary(m => m.Key, m => m.Value);

            var serializedSchema = SerializeObject(methods);
            //await SaveToFileAsync(serializedSchema, "methods");
        }

        private static string SerializeObject(object schema)
        {
            return JsonConvert.SerializeObject(schema, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            });
        }

        private static async Task SaveToFileAsync(string data, string prefix)
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var filePath = Path.Combine(desktopPath, $"{prefix}-{DateTime.Now:HHmmss}.json");

            await File.WriteAllTextAsync(filePath, data);
        }
    }
}
