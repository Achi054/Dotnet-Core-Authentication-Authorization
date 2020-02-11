var configuration = {
    userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
    authority: "https://localhost:44326/",
    client_id: "054_js",
    redirect_uri: "https://localhost:44340/home/signin",
    post_logout_redirect_uri: "https://localhost:44340/home/index", 
    response_type: "id_token token",
    scope: "openid ApiOne rc_pub_scope"
};

var userManager = new Oidc.UserManager(configuration);

var signIn = function () {
    userManager.signinRedirect();
};

var signOut = function () {
    userManager.signoutRedirect();
};

userManager.getUser().then(user => {
    console.log("User: ", user);
    if (user)
        axios.defaults.headers.common['Authorization'] = "Bearer" + user.access_token;
});

var callApi = function () {
    axios.get("https://localhost:44311/message")
        .then(message => {
            console.log(message.content);
        });
};

var refreshing = false;
// Add a response interceptor
axios.interceptors.response.use(
    function (response) { return response; },
    function (error) {
        var config = error.response.config;

        if (error.response.status === 401)
        {
            if (!refreshing)
                refreshing = !refreshing;

            return userManager.signinSilent().then(user => {
                axios.defaults.headers.common['Authorization'] = "Bearer" + user.access_token;
                config.headers.common['Authorization'] = "Bearer" + user.access_token;
                return axios(config);
            });
        }

        return Promise.reject(error);
});