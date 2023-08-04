package admin

import (
	"fmt"

	"github.com/supertokens/supertokens-golang/recipe/userroles"
)

func CreateRole(roleName string, permissions ...string) bool {
	fmt.Printf("Creating role: %s with permissions: %s \n", roleName, permissions)
	resp, err := userroles.CreateNewRoleOrAddPermissions(roleName, permissions, nil)

	if err != nil {
		panic(err.Error())
	}

	return resp.OK.CreatedNewRole
}
