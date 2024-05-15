using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResponsePackage
{
    public enum ResponseCodes
    {
        //200
        LoginSuccess = 200,
        TokenRefreshed,
        TokenRevoked,
        DataFound,
        DataCreated,
        DataDeleted,
        DataUpdated,
        FileUploaded,
        //400
        BadLoginRequest = 400,
        UserNotFound,
        BadTokenRequest,
        ValidationError,
        EmptySequence,
        Unauthorized,
        ObjectNotFound,
        ObjectNotUpdated,
        NotUnique,
        Restricted,
        //500
        UndefinedServerException = 500
    }
}
