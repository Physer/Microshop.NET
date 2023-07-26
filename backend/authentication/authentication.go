package main

import (
	"fmt"
	"log"
	"net/http"
	"os"
	"strings"

	"github.com/joho/godotenv"
	"github.com/supertokens/supertokens-golang/recipe/dashboard"
	"github.com/supertokens/supertokens-golang/recipe/emailpassword"
	"github.com/supertokens/supertokens-golang/recipe/session"
	"github.com/supertokens/supertokens-golang/supertokens"
)

var superTokensCoreUrl string
var superTokensBackendHost string
var superTokensBackendPort string
var gatewayUrl string
var websiteUrl string

func main() {
	envLoadErr := godotenv.Load("../../.env")
	if envLoadErr != nil {
		log.Fatal("Error loading .env file")
	}
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

	http.ListenAndServe(fmt.Sprintf("%s:%s", superTokensBackendHost, superTokensBackendPort), corsMiddleware(
		supertokens.Middleware(http.HandlerFunc(func(rw http.ResponseWriter,
			r *http.Request) {
		}))))
}

func corsMiddleware(next http.Handler) http.Handler {
	return http.HandlerFunc(func(response http.ResponseWriter, r *http.Request) {
		response.Header().Set("Access-Control-Allow-Origin", websiteUrl)
		response.Header().Set("Access-Control-Allow-Credentials", "true")
		if r.Method == "OPTIONS" {
			response.Header().Set("Access-Control-Allow-Headers",
				strings.Join(append([]string{"Content-Type"},
					supertokens.GetAllCORSHeaders()...), ","))
			response.Header().Set("Access-Control-Allow-Methods", "*")
			response.Write([]byte(""))
		} else {
			next.ServeHTTP(response, r)
		}
	})
}

func setEnvironmentVariables() {
	superTokensCoreUrl = os.Getenv("AUTHENTICATION_SERVICE_URL")
	superTokensBackendHost = os.Getenv("AUTHENTICATION_BACKEND_HOST")
	superTokensBackendPort = os.Getenv("AUTHENTICATION_BACKEND_PORT")
	gatewayUrl = os.Getenv("GATEWAY_URL")
	websiteUrl = os.Getenv("WEBSITE_URL")
}
