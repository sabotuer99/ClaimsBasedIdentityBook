//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Samples.Web.UI.Controls
{
    using System.ComponentModel;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using AjaxControlToolkit;
    using Samples.Web.UI.Controls.Properties;

    [DefaultProperty("Text")]
    [ToolboxData("<{0}:TooltipInformation runat=server></{0}:TooltipInformation>")]
    public class TooltipInformation : WebControl
    {
        private Image infoImage;
        private Panel tooltipContent;
        private HoverMenuExtender tooltipExtender;

        [Bindable(true)]
        [Browsable(true)]
        [Category("Content")]
        [Description("The text that displayed in the tooltip.")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                var s = (string)this.ViewState["Text"];
                return s ?? string.Empty;
            }

            set { this.ViewState["Text"] = value; }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            var infoImgUrl = this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "Samples.Web.UI.Controls.Images.info-icon.jpg");
            this.infoImage = new Image { ID = this.ID + "InfoImage" };
            this.infoImage.Attributes["src"] = infoImgUrl;
            this.infoImage.Attributes["style"] = Resources.TooltipIconStyle;
            this.Controls.Add(this.infoImage);

            this.tooltipContent = new Panel { ID = this.ID + "TooltipContent" };
            this.tooltipContent.Attributes["style"] = Resources.TooltipContentStyle;
            this.tooltipContent.Controls.Add(new LiteralControl(this.Text));
            this.Controls.Add(this.tooltipContent);

            this.tooltipExtender = new HoverMenuExtender
                                       {
                                           TargetControlID = this.infoImage.ID, 
                                           PopupControlID = this.tooltipContent.ID, 
                                           OffsetX = 15, 
                                           PopupPosition = HoverMenuPopupPosition.Right, 
                                           PopDelay = 50
                                       };
            this.Controls.Add(this.tooltipExtender);
        }
    }
}