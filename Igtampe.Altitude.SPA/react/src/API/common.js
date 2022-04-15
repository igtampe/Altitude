//The API URL. Either the one provided by the environment variable, or the default
export const APIURL = !process.env.NODE_ENV || process.env.NODE_ENV === 'development' ? 'https://localhost:7069' : '';
const EmptyGUID = ""

export const GenerateJSONPost = (SessionID, Body) => {
    return({
        method: 'POST',
        headers: { 
            'Content-Type': 'application/json', 
            'SessionID': SessionID ?? EmptyGUID },
        body: JSON.stringify(Body)
    })
}

export const GeneratePost = (SessionID, Body) => {
    return({
        method: 'POST',
        headers: { 
            'Content-Type': 'application/json', 
            'SessionID': SessionID  ?? EmptyGUID },
        body: Body
    })
}

export const GenerateJSONPut = (SessionID,Body) => {
    return({
        method: 'PUT',
        headers: { 
            'Content-Type': 'application/json', 
            'SessionID': SessionID ?? EmptyGUID},
        body: JSON.stringify(Body)
    })
}

export const GeneratePut = (SessionID,Body) => {
    return({
        method: 'PUT',
        headers: { 
            'Content-Type': 'application/json', 
            'SessionID': SessionID ?? EmptyGUID},
        body: Body
    })
}

export const GenerateGet = (SessionID) => {    
    return(SessionID ? {
        method: 'GET',
        headers: {'SessionID': SessionID ?? EmptyGUID },
    } : { method: 'GET'})
}

export const GenerateDelete = (SessionID) => {    
    return({
        method: 'DELETE',
        headers: {'SessionID': SessionID ?? EmptyGUID },
    })
}