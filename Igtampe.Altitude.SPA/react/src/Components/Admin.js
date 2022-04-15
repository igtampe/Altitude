import * as React from 'react';
import UserDisplay from './AdminComponents/UserDisplay';

export default function Admin(props) {

    return (<>
        <h1>User Administration</h1>
        <UserDisplay {...props}/>
    </>);
}