import React from "react";
import { GenerateGet, APIURL} from "./common";

export const GetImageInfo = (setLoading, ID, setImage, setError) => {

    setLoading(true);
    fetch(APIURL + "/API/Images/" + ID + "/info", GenerateGet(null))
        .then(response => response.json()).then(data => {

            //Remember to check for errors and set errors if needed
            if (data.error || data.errors) {
                setError(data.reason ?? "An unknown serverside error occurred");
                return;
            }

            setImage(data)
            setLoading(false)

        })
}

export const APIImage = (props) => {
    if(props.image){ return(<img alt={props.image.id} src={APIURL + '/API/Images/' + props.image.id} {...props}/>) }
    if(props.id){ return(<img alt={props.id} src={APIURL + '/API/Images/' + props.id} {...props}/>) }    
    return(<img alt='Unkown' src={'icons/images.png'} {...props}/>)
}

export const UploadImage = (setLoading, Session, file, onSuccess, onError) => {

    setLoading(true);

    const FR = new FileReader();
    FR.addEventListener('load', (event) => {
        setLoading(true)

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': file.type, 'Content-Length': file.size, 'SessionID': Session },
            body: event.target.result
        };

        fetch(APIURL + "/API/Images", requestOptions)
            .then(response => response.json()).then(data => {

                setLoading(false)
                //Remember to check for errors and set errors if needed
                if (data.error || data.errors) {
                    onError(data.reason ?? "An unknown serverside error occurred");
                    return;
                }

                onSuccess(data)
            })
    })

    FR.readAsArrayBuffer(file)

}