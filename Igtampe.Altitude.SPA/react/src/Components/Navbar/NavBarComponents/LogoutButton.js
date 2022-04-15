import React from "react";
import { useHistory } from "react-router-dom";
import { IconButton } from "@mui/material";
import {Logout} from '@mui/icons-material'
import { LogOut } from "../../../API/User";


export default function LogoutButton() {

    const h = useHistory();
    const Reload = () => { h.go("/") }
    const handleLogout = (event) => { LogOut(Reload); }
    
    return ( <React.Fragment> <IconButton color="inherit" onClick={handleLogout}><Logout/></IconButton> </React.Fragment> );

}
