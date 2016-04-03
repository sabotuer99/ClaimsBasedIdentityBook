//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================





namespace Samples.Web.UI.Controls.Mvc
{
    using System.Globalization;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.UI;
    using Properties;

    public static class TooltipExtension
    {
        private const string ImageIdExpression = "TooltipImage{0}";

        private const string TextContainerIdExpression = "TooltipTextContainer{0}";

        private const string TooltipJQueryBehaviorInitialization =
            "<script type='text/javascript'>tooltips.push(['{0}','{1}']);</script>";

        public static string Tooltip(this HtmlHelper htmlHelper, string text)
        {
            var page = new Page();
            
            var builderContainer = new TagBuilder("span");

            int tooltipNumber;
            if (htmlHelper.ViewData["lastTooltipNumber"] != null)
            {
                tooltipNumber = (int)htmlHelper.ViewData["lastTooltipNumber"];
            }
            else
            {
                tooltipNumber = 0;
            }

            // Background image
            var builderBackgroundImage = new TagBuilder("img");
            var imageElementId = string.Format(CultureInfo.InvariantCulture, ImageIdExpression, tooltipNumber);
            builderBackgroundImage.MergeAttribute("id", imageElementId);
            var infoImgUrl = page.ClientScript.GetWebResourceUrl(typeof(TooltipExtension), "Samples.Web.UI.Controls.Mvc.Images.info-icon.jpg");
            builderBackgroundImage.MergeAttribute("src", infoImgUrl);
            builderBackgroundImage.MergeAttribute("style", Resources.TooltipIconStyle);

            builderContainer.InnerHtml = builderBackgroundImage.ToString(TagRenderMode.SelfClosing);
            
            // Text container
            var textContainer = new TagBuilder("div");
            var textContainerElementId = string.Format(CultureInfo.InvariantCulture, TextContainerIdExpression, tooltipNumber);
            textContainer.MergeAttribute("id", textContainerElementId);
            textContainer.MergeAttribute("style", Resources.TooltipContentStyle);
            textContainer.InnerHtml = text;
            textContainer.InnerHtml += AddScripts(htmlHelper, page.ClientScript, imageElementId, textContainerElementId);
            builderContainer.InnerHtml += textContainer.ToString(TagRenderMode.Normal);

            htmlHelper.ViewData["lastTooltipNumber"] = tooltipNumber + 1;

            return builderContainer.ToString(TagRenderMode.Normal);
        }

        private static string AddScripts(HtmlHelper htmlHelper, ClientScriptManager scriptManager, string targetElementId, string popupElementId)
        {
            var scriptsBuilder = new StringBuilder();
            if (htmlHelper.ViewData["TooltipJqueryBehaviorRegistered"] == null || !(bool)htmlHelper.ViewData["TooltipJqueryBehaviorRegistered"])
            {
                var tooltipJQueryBehaviorUrl = scriptManager.GetWebResourceUrl(typeof(TooltipExtension), "Samples.Web.UI.Controls.Mvc.Scripts.TooltipJQueryBehavior.js");
                scriptsBuilder.AppendFormat("<script type='text/javascript' src='{0}' ></script>", tooltipJQueryBehaviorUrl);
                htmlHelper.ViewData["TooltipJqueryBehaviorRegistered"] = true;
            }

            scriptsBuilder.AppendFormat(TooltipJQueryBehaviorInitialization, targetElementId, popupElementId);

            return scriptsBuilder.ToString();
        }
    }
}
