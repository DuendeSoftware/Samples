import React, { Component } from "react";
import { Route } from "react-router";
import { Home } from "./components/Home";
import { Layout } from "./components/Layout";
import { UserSession } from "./components/UserSession";
import "./custom.css";

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Route exact path="/" component={Home} />
        <Route path="/user-session" component={UserSession} />
      </Layout>
    );
  }
}
