package admin

import (
	"bytes"
	"fmt"
	"net/http"
	"os"

	"github.com/supertokens/supertokens-golang/recipe/userroles"
)

func CreateDashboardUser(supertokensCoreUrl string) *http.Response {
	dashboardUserEmail := os.Getenv("DASHBOARD_USER_EMAIL")
	dashboardUserPassword := os.Getenv("DASHBOARD_USER_PASSWORD")
	requestBody := `{"email": "%s","password": "%s"}`
	formattedRequestBody := fmt.Sprintf(requestBody, dashboardUserEmail, dashboardUserPassword)
	bodyData := []byte(formattedRequestBody)
	bodyReader := bytes.NewReader(bodyData)
	formattedUrl := fmt.Sprintf("%s/recipe/dashboard/user", supertokensCoreUrl)
	fmt.Printf("Creating user: %s in authentication service through the Dashboard API: %s \n", dashboardUserEmail, formattedUrl)
	httpRequest, _ := http.NewRequest(http.MethodPost, formattedUrl, bodyReader)
	httpRequest.Header.Add("Content-Type", "application/json")
	httpRequest.Header.Add("rid", "dashboard")
	client := http.DefaultClient
	res, err := client.Do(httpRequest)
	if err != nil {
		panic(err.Error())
	}

	return res
}

func AddRoleToUser(userId string, roleName string) bool {
	fmt.Printf("Adding role %s to user %s \n", roleName, userId)
	response, err := userroles.AddRoleToUser("public", userId, roleName, nil)
	if err != nil {
		panic(err.Error())
	}

	return !response.OK.DidUserAlreadyHaveRole && response.UnknownRoleError == nil
}
