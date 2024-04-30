import axios from 'axios'
import React, { useEffect, useState } from 'react'
import { toast } from 'react-toastify'
import giphy from '../../giphyy.gif'
import NavBar from '../pages/NavBar'

function OrderPage({ orderId }) {
  const [orderItems, setOrderItems] = useState([])
  const [showDelivered, setShowDelivered] = useState(false)

  useEffect(() => {
    loadOrderItems()
  }, [])

  const loadOrderItems = async () => {
    try {
      const userId = sessionStorage.getItem('userID')
      const response = await axios.get(
        `https://localhost:7253/api/Order/${userId}`
      )
      setOrderItems(response.data)
    } catch (error) {
      console.error('Error loading order items:', error)
      toast.error('Error loading order items')
    }
  }

  const formatCurrency = (value) => {
    return new Intl.NumberFormat('en-IN', {
      maximumFractionDigits: 2,
      minimumFractionDigits: 2,
    }).format(value)
  }

  const handleShowDelivered = () => {
    setShowDelivered((prevShowDelivered) => !prevShowDelivered)
  }

  return (
    <div>
      <NavBar />
      <div className='text-center'>
        <button className='btn btn-primary mt-3' onClick={handleShowDelivered}>
          {showDelivered ? 'Back' : 'Show Delivered Orders'}
        </button>
      </div>
      {orderItems.length === 0 ? (
        <div className='container-fluid' style={{ minHeight: '80vh' }}>
          <div
            className='row justify-content-center align-items-center'
            style={{ minHeight: 'inherit' }}
          >
            <div className='col-12 text-center'>
              <img src={giphy} alt='Empty cart emo' style={{ width: '10vw' }} />
              <h5>Looks like you haven't placed an order yet.</h5>
            </div>
          </div>
        </div>
      ) : (
        <div className='row row-cols-1 row-cols-md-2 mx-md-1 mt-3'>
          {orderItems.map((item) => {
            // Check if the order status is not "Placed" or if showDelivered is true
            if (showDelivered || item.orderStatus !== 'Delivered') {
              return (
                <div key={item.id} className='col-md-6 mb-3'>
                  <div className='card'>
                    <div className='row no-gutters'>
                      <div className='col-md-5'>
                        <img
                          src={item.products.imageUrl}
                          className='card-img'
                          alt={item.products.name}
                          style={{
                            height: '100%',
                            width: '100%',
                            objectFit: 'contain',
                          }}
                        />
                      </div>
                      <div className='col-md-7'>
                        <div className='card-body'>
                          <h5
                            className='card-title mb-4'
                            style={{ color: 'green' }}
                          >
                            <img
                              src='https://upload.wikimedia.org/wikipedia/commons/8/8b/Eo_circle_green_white_checkmark.svg'
                              alt='logo'
                              className='img-fluid'
                              style={{ height: '30px', width: '30px' }}
                            />
                            <strong style={{ marginLeft: '1rem' }}>
                              Congratulations. Your order has been successfully
                              placed.
                            </strong>
                          </h5>
                          <h4 className='card-text ' title={item.products.name}>
                            <strong>
                              {item.products.name.length > 23
                                ? `${item.products.name.substring(0, 23)}...`
                                : item.products.name}
                            </strong>
                          </h4>
                          <h5 className='card-text'>
                            Order No. : {item.orderNo}
                          </h5>
                          <p className='card-text'>
                            <span style={{ fontSize: '18px' }}>
                              <strong>Order Total</strong> : ₹{' '}
                            </span>
                            <strong style={{ fontSize: '2rem' }}>
                              {formatCurrency(item.totalPrice)}
                            </strong>{' '}
                            <br />
                            <b>Manufacturer</b> : {item.products.manufacturer}{' '}
                            <br />
                            <b>Quantity</b> : {item.quantity} <br />
                            <b>Order Status</b> : {item.orderStatus} <br />
                          </p>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              )
            }
            return null
          })}
        </div>
      )}
    </div>
  )
}

export default OrderPage
