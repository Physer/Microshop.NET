package main

import (
	"fmt"
	"net/http"
	"os"
	"strconv"
	"strings"

	"github.com/joho/godotenv"
	"github.com/supertokens/supertokens-golang/recipe/dashboard"
	"github.com/supertokens/supertokens-golang/recipe/emailpassword"
	"github.com/supertokens/supertokens-golang/recipe/session"
	"github.com/supertokens/supertokens-golang/supertokens"
)

var superTokensCoreUrl string
var superTokensBackendPort int
var gatewayUrl string
var websiteUrl string

func main() {
	fmt.Println("Starting authentication service")
	fmt.Println("Loading environment variables from .env file or the environment")
	godotenv.Load("../../.env")
	setEnvironmentVariables()

	apiBasePath := "/auth"
	websiteBasePath := "/auth"
	err := supertokens.Init(supertokens.TypeInput{
		Supertokens: &supertokens.ConnectionInfo{
			ConnectionURI: superTokensCoreUrl,
		},
		AppInfo: supertokens.AppInfo{
			AppName:         "Microshop.NET",
			APIDomain:       gatewayUrl,
			WebsiteDomain:   websiteUrl,
			APIBasePath:     &apiBasePath,
			WebsiteBasePath: &websiteBasePath,
		},
		RecipeList: []supertokens.Recipe{
			emailpassword.Init(nil),
			session.Init(nil),
			dashboard.Init(nil),
		},
	})

	if err != nil {
		panic(err.Error())
	}

	fmt.Println("Initialized Supertokens")
	backendAddress := fmt.Sprintf(":%d", superTokensBackendPort)
	fmt.Printf("Listening on port %d \n", superTokensBackendPort)
	http.ListenAndServe(backendAddress, corsMiddleware(
		supertokens.Middleware(http.HandlerFunc(func(rw http.ResponseWriter,
			r *http.Request) {
		}))))
}

func corsMiddleware(next http.Handler) http.Handler {
	fmt.Println("Configuring middleware")
	return http.HandlerFunc(func(response http.ResponseWriter, r *http.Request) {
		fmt.Printf("Incoming %s request to %s \n", r.Method, r.URL)
		response.Header().Set("Access-Control-Allow-Origin", websiteUrl)
		response.Header().Set("Access-Control-Allow-Credentials", "true")
		if r.Method == "OPTIONS" {
			response.Header().Set("Access-Control-Allow-Headers",
				strings.Join(append([]string{"Content-Type"},
					supertokens.GetAllCORSHeaders()...), ","))
			response.Header().Set("Access-Control-Allow-Methods", "*")
			response.Write([]byte(""))
		} else {
			fmt.Println("Serving response")
			next.ServeHTTP(response, r)
		}
	})
}

func setEnvironmentVariables() {
	fmt.Println("Setting environment variables")
	for _, element := range os.Environ() {
		fmt.Printf("Loaded: %s \n", element)
	}
	superTokensCoreUrl = os.Getenv("AUTHENTICATION_CORE_URL")
	superTokensBackendPort, _ = strconv.Atoi(os.Getenv("AUTHENTICATION_BACKEND_PORT"))
	gatewayUrl = os.Getenv("GATEWAY_URL")
	websiteUrl = os.Getenv("WEBSITE_URL")

	fmt.Printf("Loaded %d environment variables \n", len(os.Environ()))
}
