import React, { Component } from "react";
import { Badge, ListGroup, Row, Col } from "react-bootstrap";

export default class LeaderBoard extends Component {
  render() {
    return (
      <div>
        <ListGroup>
          {this.props.list &&
            this.props.list.map((row, i, arr) => {
              return (
                <ListGroup.Item>
                  <Row>
                    <Col>
                      <span>{row.Key.Name}</span>
                      <Badge>
                        {row.Key.LeaderBoardStatus == 2 && (
                          <i className="fa fa-arrow-up" aria-hidden="true"></i>
                        )}
                        {row.Key.LeaderBoardStatus == 3 && (
                          <i
                            className="fa fa-arrow-down"
                            aria-hidden="true"
                          ></i>
                        )}
                      </Badge>
                    </Col>

                    <Col>
                      کل چت ها :
                      <Badge variant="primary">
                        {row.Key.TotalChats ? (
                          row.Key.TotalChats
                        ) : (
                          <span>0</span>
                        )}
                      </Badge>
                    </Col>
                    <Col>
                      کل مشتریان :
                      <Badge variant="info">
                        {row.Key.TotalCustomersChat ? (
                          row.Key.TotalCustomersChat
                        ) : (
                          <span>0</span>
                        )}
                      </Badge>
                    </Col>
                  </Row>
                </ListGroup.Item>
              );
            })}
        </ListGroup>
      </div>
    );
  }
}
