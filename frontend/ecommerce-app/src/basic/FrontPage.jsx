import React from 'react'
import { Link } from 'react-router-dom'
import backimage from '../backimg.jpg'

function FrontPage() {
  return (
    <div
      className='position-fixed top-0 start-0 d-flex flex-column justify-content-start align-items-end p-4'
      style={{
        backgroundImage: `url(${backimage})`,
        backgroundSize: 'cover',
        backgroundPosition: 'center',
        height: '100vh',
        width: '100vw',
        zIndex: 9999,
      }}
    >
      <div
        className='mb-auto text-white ms-auto'
        style={{ margin: '9rem 7rem' }}
      >
        <h2 style={{ fontFamily: 'Roboto Mono, sans-serif' }}>
          Unlock the Door to <br /> Shopping Delight! <br /> Click Here to{' '}
          <br /> Login and Start <br />
          Exploring
        </h2>
        <div className='mt-4 d-grid gap-2 col-9'>
          <button className='btn btn-primary'>
            <Link
              to='/login'
              className='text-white text-decoration-none'
              style={{ fontFamily: 'Roboto Mono, sans-serif' }}
            >
              Login
            </Link>
          </button>
        </div>
      </div>
    </div>
  )
}

export default FrontPage
