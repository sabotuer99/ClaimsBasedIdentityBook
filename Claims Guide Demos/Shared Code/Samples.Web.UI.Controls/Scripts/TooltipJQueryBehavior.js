var tooltips = new Array();
var horizontalOffset = 20;
var verticalOffset = 5;

function InitializeTooltip(targetElementId, popupElementId) {
    var targetElement = $("#" + targetElementId);
    var popupElement = $("#" + popupElementId);

    targetElement.hover(
        function() {
            var targetElementPosition = targetElement.offset();
            popupElement.css('top', targetElementPosition.top - verticalOffset);
            popupElement.css('left', targetElementPosition.left + horizontalOffset);
            popupElement.show();
        },
        function() {
            popupElement.hide();
        });
}

$(document).ready(function() {
    for (var i in tooltips) {
        InitializeTooltip(tooltips[i][0], tooltips[i][1]);
    }
});