//om nom nom. You're too late. I ate the hamburger. Have this menu instead.
import { Box, Divider, Drawer, List, ListItem, ListItemIcon, ListItemText, Switch } from "@mui/material";
import React from "react";


export default function Hamburger(props) {

    const GenerateListItem = (props) => {
      return (
        <ListItem button key={props.text} onClick={() => props.PushTo(props.url)}>
          <ListItemIcon><img src={"/icons/" + (props.DarkMode ? "white/" : "black/") + props.image} alt={props.imageAlt} width="30px" style={{ margin: "5px", marginLeft: "10px" }} /></ListItemIcon>
          <ListItemText>{props.text}</ListItemText>
        </ListItem>
      );
    }
  
    const PushTo = (url) => {
      props.history.push(url);
      props.setMenuOpen(false)
    }
  
    return (
      <Drawer open={props.menuOpen} onClose={() => props.setMenuOpen(false)}>
        <Box
          sx={{ width: 250 }}
          role="presentation"
        >
          <List>
            <ListItem key="Logo">
              <div style={{ textAlign: 'center', width: '100%' }}>
                <img src="/logobg.png" alt="Horizon logo" height="150" />
              </div>
            </ListItem>
          </List>
          <Divider />
  
          { props.Session ? <>
                <GenerateListItem text='My Trips' url='/Trips' image='add.png' imageAlt='Add' DarkMode={props.DarkMode} PushTo={PushTo} />
                <Divider />
                <List>
                  {
                    props.User && (props.User.isAdmin)
                      ? <GenerateListItem text='Administrate' url='/Admin' image='admin.png' imageAlt='Admin' DarkMode={props.DarkMode} PushTo={PushTo} />
                      : <></>}
  
                </List>
                <Divider />
              </>
              : <List>
                <GenerateListItem text='Login' url='/Login' image='person.png' imageAlt='Person' DarkMode={props.DarkMode} PushTo={PushTo} />
              </List>
          }
          <Divider />
          <List>
            <ListItem>
              <ListItemText style={{ textAlign: "center" }}>
                Dark Mode:
                <Switch checked={props.DarkMode} onChange={() => props.ToggleDarkMode()} />
              </ListItemText>
            </ListItem>
          </List>
    
        </Box>
      </Drawer>
    )
  
  
  }
  