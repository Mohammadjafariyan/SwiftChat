import React, { Component } from "react";
import { Badge, ListGroup, Row, Col } from "react-bootstrap";

export default class LeaderBoard extends Component {
  render() {
    return (
      <div>
        <ListGroup>
          {this.props.list &&
            this.props.list.map((row, i, arr) => {
              let j=i+1;
              return (
                <ListGroup.Item>
                  <Row>
                  <Col>

<button className='btn btn-success' style={{borderRadius:'45%'}}>{j}</button>
{i==0 && <>
  <i className="fa fa-star" style={{color:'#d7da1b'}}></i>
  <i className="fa fa-star" style={{color:'#d7da1b'}}></i>
  <i className="fa fa-star" style={{color:'#d7da1b'}}></i>

</>}

{i==1 && <>
  <i className="fa fa-star" style={{color:'#d7da1b'}}></i>
  <i className="fa fa-star" style={{color:'#d7da1b'}}></i>

</>}

{i==2 && <>
  <i className="fa fa-star" style={{color:'#d7da1b'}}></i>

</>}

</Col>
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
                        {row.TotalChats ? (
                          <span>{row.TotalChats}</span>
                        ) : (
                          <span>0</span>
                        )}
                      </Badge>
                    </Col>
                    <Col>
                      کل مشتریان :
                      <Badge variant="info">
                        {row.TotalCustomersChat ? (
                          <span>{row.TotalCustomersChat}</span>
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
