$(document).ready(async function() {
    var input = $("#input-text-area");
    var output = $("#output-text-area");
    var sortAttributes = $("#sort-attributes");
    var generateInvalidTags = $("#generate-invalid-tags");

    input.linenumbers({ col_width: "25px", start: 1, digits:4});

    update = async function() {
        $.ajax({
            url: "/api/translator",
            type: "POST",
            data: JSON.stringify(
                {
                    "Html": input.val(),
                    "GenerateInvalidTags": generateInvalidTags.is(":checked"),
                    "SortAttributes": sortAttributes.is(":checked"),
                }),
            contentType: "application/json",
            dataType: "json",
            success: function(data, textStatus, jqXHR) {
                output.val(data.RazorSharp);
                if (data.Error) {
                    output.css("background-color", "#ffa07aff");
                } else {
                    output.css("background-color", "white");
                }
            }
        });
    };

    input.on("input", update);
    sortAttributes.on("change", update);
    generateInvalidTags.on("change", update);

    window.onresize = function (event) {
        location.reload();
    };

})

window.onbeforeunload = function () {
    localStorage.setItem("input", $("#input-text-area").val());
    localStorage.setItem("output", $("#output-text-area").val());
    localStorage.setItem("sortAttributes", $("#sort-attributes").is(":checked"));
    //alert($("#sort-attributes").is(":checked"));
    localStorage.setItem("generateInvalidTags", $("#generate-invalid-tags").is(":checked"));
}

window.onload = function () {
    var inputVal = localStorage.getItem("input");
    if (inputVal !== null) $("#input-text-area").val(inputVal);

    var outputVal = localStorage.getItem("output");
    if (outputVal !== null) $("#output-text-area").val(outputVal);

    $("#sort-attributes").prop("checked", localStorage.getItem("sortAttributes") == 'true');
    $("#generate-invalid-tags").prop("checked", localStorage.getItem("generateInvalidTags") == 'true');
}