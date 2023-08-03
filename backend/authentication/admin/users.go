package admin

import (
	"bytes"
	"fmt"
	"net/http"
	"os"
)

func CreateDashboardUser(supertokensCoreUrl string) (*http.Response, error) {
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

	return res, err
}
