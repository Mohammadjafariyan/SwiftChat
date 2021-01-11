import React, { Component } from "react";
import { Row } from "react-bootstrap";
import { Col ,Button} from "react-bootstrap";
import { MyCaller } from './../../Help/Socket';
import {CurrentUserInfo} from "../../CurrentUserInfo";

export default class CustomersPaging extends Component {
  state = {};
  constructor(props) {
    super(props);
    CurrentUserInfo.CustomersPaging = this;
  }


  componentDidMount(){
  }

  

  GetClientsListForAdmin(Page) {
    if (CurrentUserInfo.MapPage)
      CurrentUserInfo.MapPage.setState({ loading: true });

    if (CurrentUserInfo.OnlineCustomerListHolder)
      CurrentUserInfo.OnlineCustomerListHolder.setState({ loading: true });

    MyCaller.Send("GetClientsListForAdmin", {
      userType: CurrentUserInfo.UserType,
      gapIsOnlyOnly: CurrentUserInfo.gapIsOnlyOnly,
      Page: Page,
    });
  }

  getClientsListForAdminCallback(res) {

    this.setState({ loading: false });

    if (!res || !res.Content || !res.Content.EntityList) {
      CurrentUserInfo.LayoutPage.showError(
        "getClientsListForAdminCallback returns null"
      );
      return;
    }

    var arr = [];
    arr = res.Content.EntityList;

    this.setState({
      Page: res.Content.Page,
      TotalPages: res.Content.TotalPages,
      Total: res.Content.Total,
      rn2: Math.random(),
    });

  }

  onPrev() {
    let page = this.state.Page ? this.state.Page : 0;
    page--;
    if (page < 0) {
      page = 0;
    }

    this.GetClientsListForAdmin(page);
  }

  onNext() {
    let page = this.state.Page ? this.state.Page : 0;
    let TotalPages = this.state.TotalPages ? this.state.TotalPages : 0;
    page++;
    if (page > TotalPages) {
      page = TotalPages;
    }

    this.GetClientsListForAdmin(page);
  }
  render() {
    console.log('CurrentUserInfo.CustomersPaging->state',this.state);
    return (
    <>
  <Row>
        <Col>
          <Button variant='light'
            onClick={() => {
              this.onPrev();
            }}
          >
            قبلی
          </Button>
        </Col>
        <Col md={6}>
          <span>نمایش </span>
          <span className="p-badge"> {this.state.Page} </span>
          <span> از کل </span>
          <span> {this.state.TotalPages} </span>
          <span> صفحهات </span>
          <span> | </span>
          <span> کل رکورد ها </span>
          <span> {this.state.Total} </span>
          <span> تا </span>
        </Col>
        <Col>
          <Button variant='light'
            onClick={() => {
              this.onNext();
            }}
          >
            بعدی
          </Button>
        </Col>
      </Row>
      <hr/>

    </>
    );
  }
}
