$(document).ready(async function() {
    var input = $("#input-text-area");
    var output = $("#output-text-area");

    input.on("input",
        async function() {
            $.ajax({
                url: "/api/translator",
                type: "POST",
                data: JSON.stringify(
                    {
                        method: 'POST',
                        headers:
                        {
                            'Content-Type': 'application/json'
                        },
                        body:
                        {
                            "Html": input.val()
                        }
                    }),
                contentType: "application/json",
                dataType: "json",
                success: function(data, textStatus, jqXHR) {
                    output.val(data.RazorSharp)
                }
            });
        });
})

//var response = await fetch('/api/translator', JSON.stringify({
//    method: 'POST',
//    headers: {
//        'Content-Type': 'application/json'
//    },
//    body: {
//        "Html": input.val()
//    }
//}));

//const json = await response.json();

//alert("Input " + json.RazorSharp)

//output.val(json.RazorSharp);