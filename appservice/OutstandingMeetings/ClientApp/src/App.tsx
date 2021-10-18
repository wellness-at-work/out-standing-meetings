import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';

import './custom.css'
import SignIn from './components/Signin';
import Ranking from './components/Ranking';

export default () => (
    <Layout>
        <Route exact path='/' component={SignIn} />
        <Route path='/ranking' component={Ranking} />
    </Layout>
);
