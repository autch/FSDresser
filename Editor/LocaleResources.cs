using UnityEditor.Localization.Editor;

namespace Autch.FSDresser
{
    public class LocaleResources
    {
        public static string _(string s)
        {
#if UNITY_2020_2_OR_NEWER
            return L10n.Tr(s);
#else
            return Localization.Tr(s);
#endif
        }

        public static readonly string Title = _("FSDresser");
        public static readonly string AvatarToWear = _("Avatar to wear");
        public static readonly string NumOfItemsToWear = _("# of items to wear");
        public static readonly string AddOrRemoveItemInList = _("Add/remove items in list");
        public static readonly string ListOfItemsToWear = _("List of items to wear ({0})");
        public static readonly string ItemToWear = _("Item to wear ({0})");
        public static readonly string Verify = _("Verify");
        public static readonly string VerificationResult = _("Verification result");
        public static readonly string DoIt = _("GO");
        public static readonly string Success = _("Success!");
        public static readonly string FailedToCompileAvatar = _("Failed to compile avatar");
        public static readonly string Abort = _("Abort");

        public static readonly string AvatarDoesNotHaveAnimatorComponent =
            _("Avatar {0} does not have Animator component attached");

        public static readonly string AvatarDoesNotHaveHipsBoneDefined =
            _("Avatar {0} does not have Hips bone defined");

        public static readonly string FailedToDuplicateAvatar = _("Failed to duplicate avatar {0}");
        public static readonly string FailedToDuplicateItem = _("Failed to duplicate item {0}");

    }
}
