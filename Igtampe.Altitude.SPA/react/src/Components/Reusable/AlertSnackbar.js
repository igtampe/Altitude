import { Snackbar, Alert } from "@mui/material"
import React from "react";

export default function AlertSnackbar(props) {

    const handleClose = (event) =>{ props.setOpen(false) }

    var Severity = props.result ? props.result.severity : 'success'
    if(Severity.toLowerCase()==="danger") {Severity = 'error'} 

    return(
        <Snackbar open={props.open} autoHideDuration={6000} onClose={handleClose}>
            <Alert onClose={handleClose} severity={Severity} sx={{ width: '100%' }}>
                {props.result ? props.result.text : 'something happened!'}
            </Alert>
        </Snackbar>
    )
    
}