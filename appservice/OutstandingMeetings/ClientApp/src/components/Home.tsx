import * as React from 'react';
import { connect } from 'react-redux';
import Ranking from './Ranking';

const Home = () => (
  <Ranking></Ranking>
);

export default connect()(Home);
