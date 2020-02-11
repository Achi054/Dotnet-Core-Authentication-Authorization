var signIn = function () {
    var authUrlComponent =
        "?client_id=" + "054_js" +
        "&redirect_uri=" + encodeURIComponent("https://localhost:44340/home/signin") +
        "&response_type=" + encodeURIComponent("id_token token") +
        "&scope=" + encodeURIComponent("openid ApiOne") + 
        "&state=" + createState() +
        "&nonce=" + createNonce();

    var authUrl = encodeURIComponent(authUrlComponent);

    window.location.href =
        "https://localhost:44326/Home/Login?ReturnUrl=/connect/authorize/callback" + authUrl;

    function createState() {
        return "session_state_value_that_is_used_as_part_of_the_authentication_mechanism";
    }

    function createNonce() {
        return "nonce_value_that_is_used_as_part_of_the_authentication_mechanism";
    }
}
