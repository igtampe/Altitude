import Cookies from "universal-cookie";
import { GenerateGet, GenerateJSONPost, GenerateJSONPut, APIURL } from "./common";

export const GetUserDirectory = (setLoading, setUsers, Query, Skip, Take) => {

    setLoading(true);

    //Build the Query
    var append = undefined;
    if (Boolean(Query)) { append = "?Query=" + Query }
    if (Boolean(Skip)) {
        if (!append) { append += "?" } else { append += "&" }
        append += append + "Skip=" + Skip
    }
    if (Boolean(Take)) {
        if (!append) { append += "?" } else { append += "&" }
        append += append + "Take=" + Take
    }

    //Fetch
    fetch(APIURL + "/API/Users/Dir" + append, GenerateGet(null))
        .then(response => { return response.json() }).then(data => {

            //Remember to check for errors and set errors if needed
            if (data.error || data.errors) { return; }

            setUsers(data)
            setLoading(false)

        })

}

export const GetMe = (setLoading, Session, setUser, setInvalidSession) => {

    setLoading(true);
    fetch(APIURL + "/API/Users", GenerateGet(Session))
        .then(response => {
            if (!response.ok) {
                setInvalidSession(true)
                return undefined;
            }
            return response.json()
        }).then(data => {

            //Actually we don't need to check if it's undefined or not
            setUser(data)
            setLoading(false)
            //if it's invalid it'll just set undefined which is what user should be

        })

}

export const GetUser = (setLoading, ID, setUser, setError) => {

    setLoading(true);
    fetch(APIURL + "/API/Users/" + ID, GenerateGet(null))
        .then(response => response.json()).then(data => {

            //Remember to check for errors and set errors if needed
            if (data.error || data.errors) {
                setError(data.error ?? "An unknown serverside error occurred");
                return;
            }

            setUser(data)
            setLoading(false)

        })

}

export const ChangePassword = (setLoading, Session, currentpass, newpass, setError) => {

    setLoading(true);

    //Build the body
    var B = {
        "current": currentpass,
        "new": newpass
    }

    //Fetch
    fetch(APIURL + "/API/Users", GenerateJSONPut(Session, B))
        .then(response => { return response.json() }).then(data => {

            //Remember to check for errors and set errors if needed
            if (data.error || data.errors) {
                setError(data.error ?? "An unknown serverside error occurred");
                return;
            }

            setLoading(false)

        })

}

export const ResetPassword = (setLoading, Session, id, newpass, setError) => {

    setLoading(true);

    //Build the body
    var B = {
        "current": "",
        "new": newpass
    }

    //Fetch
    fetch(APIURL + "/API/Users" + id + "/Reset", GenerateJSONPut(Session, B))
        .then(response => { return response.json() }).then(data => {

            //Remember to check for errors and set errors if needed
            if (data.error || data.errors) {
                setError(data.error ?? "An unknown serverside error occurred");
                return;
            }

            setLoading(false)

        })

}

export const ChangePFP = (setLoading, Session, imageURL, setError) => {

    setLoading(true);

    //Fetch
    fetch(APIURL + "/API/Users/image", GenerateJSONPut(Session, imageURL))
        .then(response => { return response.json() }).then(data => {

            //Remember to check for errors and set errors if needed
            if (data.error || data.errors) {
                setError(data.error ?? "An unknown serverside error occurred");
                return;
            }

            setLoading(false)

        })

}

export const Register = (setLoading, username, password, setError, onSuccess) => {
    setLoading(true)

    fetch(APIURL + "/API/Users/register", GenerateJSONPost(null, {
        "username": username, "password": password 
    })).then(a => a.json()).then(data => {
        setLoading(false)

        if (data.error || data.errors) {
            setError(data.reason ?? "An unknown serverside error occurred");
            return;
        }

        onSuccess()
    })
}

export const LogIn = (setLoading, username, password, setData) => {
    setLoading(true)

    fetch(APIURL + "/API/Users", GenerateJSONPost(null, {
        "username": username, "password": password
    }))
        .then(response => {  return response.json() }).then(data => {
            setData(data)
            setLoading(false)
        })
}

export const LogOut = (refreshUser) => {

    const cookies = new Cookies();

    fetch(APIURL + "/API/Users/out", GenerateJSONPost(cookies.get('SessionID'),cookies.get('SessionID'))).then(response => {
        cookies.remove("SessionID", { path: '/' })
        refreshUser();
    })

}