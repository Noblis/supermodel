function supermodel_restfulLinkToUrlWithConfirmation(path, httpOverrideVerb, confirmationMsg) {
    if (confirmationMsg == null) {
        supermodel_restfulLinkToUrl(path, httpOverrideVerb);
    } else {
        bootbox.confirm(confirmationMsg, function (result) {
            if (result) supermodel_restfulLinkToUrl(path, httpOverrideVerb);
        });
    }
}

function supermodel_restfulLinkToUrl(path, httpOverrideVerb) {
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", path);

    if (httpOverrideVerb.toLowerCase() !== 'post') {
        var hiddenField = document.createElement("input");
        hiddenField.setAttribute("type", "hidden");
        hiddenField.setAttribute("name", "X-HTTP-Method-Override");
        hiddenField.setAttribute("value", httpOverrideVerb);
        form.appendChild(hiddenField);
    }

    document.body.appendChild(form);
    form.submit();
}