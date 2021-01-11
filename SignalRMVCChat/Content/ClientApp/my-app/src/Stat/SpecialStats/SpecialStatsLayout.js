import React, { Component } from "react";
import LeaderBoard from "./leaderBoard/LeaderBoard";
import Rating from "./Rating/Rating";
import HelpDeskArticleRead from "./HelpDeskArticleRead/HelpDeskArticleRead";
import CompaignSent from "./CompaignSent/CompaignSent";
import { Row } from 'react-bootstrap';
import { Col } from 'react-bootstrap';
import Comments from './Comments/Comments';
import CommentsPage from './Comments/CommentsPage';
import {CurrentUserInfo} from "../../CurrentUserInfo";

export default class SpecialStatsLayout extends Component {
  state = {};
  constructor(props) {
    super(props);
    CurrentUserInfo.SpecialStatsLayout = this;
  }

  componentDidMount(){


    if(this.props.data){
        this.getVisitedPagesForCurrentSiteCallback(this.props.data)
    }
  }

  getVisitedPagesForCurrentSiteCallback(res){


    this.setState({loading: false});

    let arr = [];

    let trackinfosViewModellist = res.Content.mostVisitedPages;
    if (!trackinfosViewModellist) {
        CurrentUserInfo.LayoutPage.showError(
            ("دیتا نال است")
        );
        alert("دیتا نال است");
        return;
    }


    this.setState({stat:res});
  }

  render() {
    if (!this.state.stat) return <></>;

    return (
      <div>

        <Rating rating={this.state.stat.Content.Rating}/>
        <br/>

        <CommentsPage/>
        <br/>

        <HelpDeskArticleRead data={this.state.stat}/>

        <CompaignSent data={this.state.stat} />

<br/>

<Row>


<Col md={7}>
<LeaderBoard list={this.state.stat.Content.LeaderBoardOperators} />

</Col>
</Row>

      </div>
    );
  }
}
