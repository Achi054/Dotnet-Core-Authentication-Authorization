var encodeToken = function (address) {
    var tokenContent = address.split('#')[1];
    var tokens = tokenContent.split('&');

    for (var i = 0; i < tokens.length; i++) {
        var kvPair = tokens[i].split('=');
        localStorage.setItem(kvPair[0], kvPair[1]);
    }
}

encodeToken(window.location.href);